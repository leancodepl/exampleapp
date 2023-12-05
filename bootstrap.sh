#!/usr/bin/env bash

set -euo pipefail
shopt -s globstar nullglob

default_source_repository=git@github.com:leancodepl/exampleapp.git

deploy_dev_cluster=1
source_repository=${default_source_repository}

function print_help()
{
    cat <<EOF
Usage:
    bootstrap.sh [-h|--help] [--dev-cluster] [--no-dev-cluster] [--source-repository=REPOSITORY] [--context=CONTEXT] PROJECT

Positional arguments:
    PROJECT: PascalCased name of the new project to bootstrap.

Options:
    -h, --help:
        Print this message and exit.

    --dev-cluster:
        Deploy dev-cluster and apply initial migration, set SENDGRID_API_KEY environment variable
        to use it when configuring the cluster to enable email sending from Kratos.

    --no-dev-cluster:
        Do not deploy dev-cluster or apply initial migration, useful when altering k3d.yaml to enable
        fuse-overlayfs snapshotter for systems that don't support overlayfs on their /var/lib/docker filesystem.

    --source-repository=REPOSITORY:
        Clone ExampleApp from REPOSITORY instead of default ${default_source_repository}.

    --context=CONTEXT:
        Set name of the first bounded context to CONTEXT. This should be a PascalCased name.
EOF
}

function set_project_name()
{
    if [[ -v project_name ]]
    then
        echo 1>&2 "${0}: More than one project name was given."
        print_help 1>&2
        exit 255
    else
        project_name=${1}
    fi
}

while (( ${#} > 0 ))
do
    case "${1}" in
        (--)
            shift
            break
        ;;
        (-h|--help)
            print_help
            exit 0
        ;;
        (--dev-cluster)
            deploy_dev_cluster=1
        ;;
        (--no-dev-cluster)
            deploy_dev_cluster=0
        ;;
        (--source-repository)
            shift
            source_repository="${1}"
        ;;
        (--source-repository=*)
            source_repository="${1##--source-repository=}"
        ;;
        (--context)
            shift
            context_name="${1}"
        ;;
        (--context=*)
            context_name="${1##--context=}"
        ;;
        (--*)
            echo 1>&2 "${0}: Unknown option '${1}'."
            print_help 1>&2
            exit 255
        ;;
        (*)
            set_project_name "${1}"
        ;;
    esac
    shift
done

while (( ${#} > 0 ))
do
    set_project_name "${1}"
    shift
done

if ! [[ -v project_name ]]
then
    echo 1>&2 "${0}: No project name was given."
    print_help 1>&2
    exit 255
fi

# Checkout the repository
git clone "${source_repository}" "${project_name,,}"
cd "${project_name,,}"
# Delete existing history, bootstrap.sh and its pipeline's Jenkinsfile
rm -rf ./.git ./bootstrap.sh ./Jenkinsfile.bootstrap

# Initialize new repository
git init
# Create initial commit
git add -A
git commit -m Bootstrap
# Ensure there are no leftovers
git clean -dffx

function rename_and_replace()
{
    # Perform directory and file renames, this operates breadth-first up to arbitrary depth of 10 levels to make sure
    # we don't attempt to rename a file in some directory that no longer exists because it has been renamed earlier
    for depth in {0..10}
    do
        # Gather all matches first to MAPFILE variable/array
        mapfile -d '' < <(find . -mindepth "${depth}" -maxdepth "${depth}" \( -not -path '*/.git/*' \) -name "*${1}*" -print0)

        for file in "${MAPFILE[@]}"
        do
            mv -T "${file}" "${file//${1}/${2}}"
        done
    done

    # Replace names in all text files, preserving casing (supports lowercase and PascalCase)
    find . \( -not -path '*/.git/*' \) -type f -execdir sed -i -e "s/${1}/${2}/g;s/${1,,}/${2,,}/g" '{}' +
}

rename_and_replace ExampleApp "${project_name}"

# Create dev cluster, if your filesystem does not support overlayfs edit k3d.yaml before this step
if (( deploy_dev_cluster ))
then
    pushd ./dev-cluster
    echo "sendgrid_api_key = \"${SENDGRID_API_KEY:-PLACEHOLDER}\"" > ./terraform.tfvars
    ./deploy.sh
    kubectl config use-context "k3d-${project_name,,}"
    popd
fi

# Remove unnecessary "lncd-" prefix from Azure resource names
sed -i 's/lncd-//g;s/ *# prefix to avoid resource FQDN collisions//g;' ./infrastructure/main.tf

pushd backend
# Remove ExampleApp-specific stuff: sample domain and related contracts/handlers/repositories/tests
rm -rf \
    ./src/Apps/*.Migrations/Migrations/* \
    ./src/Examples/*.Examples.{Contracts,Domain}/{Events,Employees,Projects} \
    ./src/Examples/*.Examples.Services/{CQRS,Processes}/{Employees,Projects} \
    ./src/Examples/*.Examples.Services/DataAccess/Repositories/{Employees,Projects}Repository.cs \
    ./tests/Examples/*/**.cs \
    ./tests/*.IntegrationTests/Example

# Remove configuration of deleted entities
sed -Ei '/^        builder\.Entity<(Employee|Project)>/,/^        \}\);$/d;' \
    ./src/Examples/*.Examples.Services/DataAccess/*DbContext.cs

# Remove DbSets, ID types, usings
sed -Ei '/(Assignment|Employee|Project)/d;' \
    ./src/Examples/*.Examples.Services/DataAccess/*DbContext.cs

# Comment out ConfigureConventions because it's an error to have a local function that's never called
sed -Ei '/ConfigureConventions/,/^    \}$/{s/^    /    \/\/ /};' \
    ./src/Examples/*.Examples.Services/DataAccess/*DbContext.cs

# Remove invalid usings and registrations of removed repositories
sed -Ei '/(Domain|Assignment|Employee|Project|Repositories)/d;' \
    ./src/Examples/*.Examples.Services/ServiceCollectionExtensions.cs

if [[ -v context_name ]]
then
    rename_and_replace Examples "${context_name}"
fi

# Fix remaining usings
dotnet tool restore
dotnet tool run dotnet-format .

# Create new migration
pushd "src/Apps/${project_name}.Migrations"
export PostgreSQL__ConnectionString="Host=localhost;Database=app;Username=app;Password=Passw12#"
dotnet tool run dotnet-ef migrations add InitialMigration -o ./Migrations

# Format everything
dotnet tool run dotnet-format .
dotnet tool run dotnet-csharpier ../../..

# Apply initial migration
if (( deploy_dev_cluster ))
then
    dotnet tool run dotnet-ef database update
fi

popd

# Build the entire backend to ensure there are no compilation errors
dotnet build
