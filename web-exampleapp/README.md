# ExampleApp - Web

Simple web app built on top of backend's example app. Showcases the frontend stack and usage of many LeanCode libraries
including:
[Admin App Generator](https://github.com/leancodepl/contractsgenerator-typescript/blob/main/packages/api-admin/README.md),
[Auth package for Ory Kratos](https://github.com/leancodepl/js_corelibrary/blob/main/packages/kratos/README.md) and
[Contracts Generator](https://github.com/leancodepl/contractsgenerator-typescript/blob/main/README.md).

## Installing dependencies

The `.nvmrc` file, which is located in the web directory, contains currently recommended version of node. It is also
highly recommended to use nvm and
[nvm/deeper-shell-integration](https://github.com/nvm-sh/nvm?tab=readme-ov-file#deeper-shell-integration) to manage node
versions.

```bash
npm i
npm start
```

## Nx

Project is managed using Nx, therefore one should refer to the [Nx docs](https://nx.dev/getting-started/intro) if
needed. You can find all defined targets in `apps/web/project.json`.

### Running the app

```bash
npx nx run web:serve
```

### Generating translations

```bash
npx nx run web:i18n
```

### Generating contracts

```bash
npx nx run web:generate
```

## Proxy

To be able to connect to the test environment's backend, you will need to use a proxy. This will require
[docker](https://docs.docker.com/manuals/). After navigating to the `dev` directory and running the command shown below,
you will be able to access a running local app at `https://local.lncd.pl/`.

```bash
docker compose up proxy
# or
docker-compose up proxy
```
