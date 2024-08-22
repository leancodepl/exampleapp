import { FormattedMessage } from "react-intl"
import { Link } from "react-router-dom"
import { Button, Card, Flex, Space } from "antd"
import { AddByNameModal } from "components/common/AddByNameModal"
import { path } from "routes"
import { useDialog } from "@leancodepl/utils"
import { api, apiComponents } from "../../api"
import { Page } from "../../components/common/Page"

export function ProjectsPage() {
    const { isDialogOpen, openDialog, closeDialog } = useDialog()

    const { mutate, isPending } = api.useCreateProject({
        invalidateQueries: [api.useAllProjectsAdmin.key({})],
    })

    return (
        <Page>
            <Card>
                <Space
                    direction="vertical"
                    size="small"
                    style={{
                        width: "100%",
                    }}>
                    <Flex justify="flex-end">
                        <Button onClick={openDialog}>
                            <FormattedMessage defaultMessage="Add project" />
                        </Button>
                    </Flex>
                    <apiComponents.AllProjectsAdminApiTable
                        NameRender={(_, { id, name }) => (
                            <Link
                                to={path("projects", "project", {
                                    projectId: id,
                                })}>
                                {name}
                            </Link>
                        )}
                    />
                </Space>
            </Card>
            <AddByNameModal
                isAdding={isPending}
                isOpen={isDialogOpen}
                title={<FormattedMessage defaultMessage="Add project" />}
                onAdd={name => {
                    mutate({
                        Name: name,
                    })
                }}
                onClose={closeDialog}
            />
        </Page>
    )
}
