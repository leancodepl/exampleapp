import { useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { useParams } from "react-router"
import { Button, Card, Flex, Select, Skeleton, Table } from "antd"
import { ColumnProps } from "antd/es/table"
import Title from "antd/es/typography/Title"
import { api } from "api"
import { ProjectDetailsDTO } from "api/cqrs"
import { AddByNameModal } from "components/common/AddByNameModal"
import { Page } from "components/common/Page"
import { Redirect } from "components/common/Redirect"
import { path } from "routes"
import { UncapitalizeDeep, useDialog } from "@leancodepl/utils"
import { ProjectName } from "./styles"

export function ProjectPage() {
    const { projectId } = useParams()

    const { isDialogOpen, openDialog, closeDialog } = useDialog()

    const { mutate: addAssignmentsToProject, isPending: areAssignmentsBeingAddedToProject } =
        api.useAddAssignmentsToProject({
            invalidateQueries: [api.useProjectDetails.key({})],
        })

    const { data: projectDetails, isLoading: isProjectLoading } = api.useProjectDetails({
        Id: projectId ?? "",
    })
    const { data: allEmployees, isLoading: areEmployeesLoading } = api.useAllEmployees({})
    const employeesOptions = useMemo(
        () => [
            ...(allEmployees?.map(({ id, name }) => ({ value: id, label: name })) ?? []),
            {
                value: notAssignedId,
                label: "-",
            },
        ],
        [allEmployees],
    )

    const { mutate: assignEmployeeToAssignment } = api.useAssignEmployeeToAssignment({})
    const { mutate: unassignEmployeeFromAssignment } = api.useUnassignEmployeeFromAssignment({})

    api.useProjectEmployeesAssignmentsTopic(
        {
            ProjectId: projectId ?? "",
        },
        {
            onData: () => {
                api.useAllEmployees.invalidate({})
                api.useProjectDetails.invalidate({})
            },
        },
    )

    const columns = useMemo<ColumnProps<Assignment>[]>(
        () => [
            {
                title: <FormattedMessage defaultMessage="Name" />,
                dataIndex: "name",
            },
            {
                title: <FormattedMessage defaultMessage="Assigned employee" />,
                width: 400,
                render: (_, { id, assignedEmployeeId }) => (
                    <Select
                        loading={areEmployeesLoading}
                        options={employeesOptions}
                        style={{
                            width: "100%",
                        }}
                        value={assignedEmployeeId ?? notAssignedId}
                        onChange={value => {
                            if (value === notAssignedId) {
                                unassignEmployeeFromAssignment({
                                    AssignmentId: id,
                                })
                                return
                            }

                            assignEmployeeToAssignment({
                                AssignmentId: id,
                                EmployeeId: value,
                            })
                        }}
                    />
                ),
            },
        ],
        [areEmployeesLoading, assignEmployeeToAssignment, employeesOptions, unassignEmployeeFromAssignment],
    )

    if (!projectId) {
        return <Redirect path={path("projects", "index")} />
    }

    return (
        <Page>
            <Card>
                <Flex justify="space-between">
                    {isProjectLoading ? (
                        <Skeleton
                            active
                            paragraph={false}
                            title={{
                                width: "30%",
                            }}
                        />
                    ) : (
                        <ProjectName level={3}>{projectDetails?.name}</ProjectName>
                    )}
                    <Button onClick={openDialog}>
                        <FormattedMessage defaultMessage="Add assignment" />
                    </Button>
                </Flex>
                <Title level={4}>
                    <FormattedMessage defaultMessage="Assignments" />
                </Title>
                <Table columns={columns} dataSource={projectDetails?.assignments ?? []} loading={isProjectLoading} />
            </Card>
            <AddByNameModal
                isAdding={areAssignmentsBeingAddedToProject}
                isOpen={isDialogOpen}
                title={<FormattedMessage defaultMessage="Add assignment" />}
                onAdd={name => {
                    addAssignmentsToProject({
                        Assignments: [
                            {
                                Name: name,
                            },
                        ],
                        ProjectId: projectId ?? "",
                    })
                }}
                onClose={closeDialog}
            />
        </Page>
    )
}

const notAssignedId = "-"

type Assignment = UncapitalizeDeep<ProjectDetailsDTO["Assignments"][number]>
