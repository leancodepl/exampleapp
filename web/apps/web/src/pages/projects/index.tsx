import { FormattedMessage } from "react-intl"
import { Link } from "react-router-dom"
import { Button, Card } from "antd"
import { AddByNameModal } from "components/common/AddByNameModal"
import { Box } from "components/common/Box"
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
                <Box
                    justify="flex-end"
                    padding={{
                        bottom: "small",
                    }}>
                    <Button onClick={openDialog}>
                        <FormattedMessage defaultMessage="Add project" />
                    </Button>
                </Box>
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
