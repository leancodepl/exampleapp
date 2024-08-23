import { FormattedMessage } from "react-intl"
import { Button, Card } from "antd"
import { AddByNameModal } from "components/common/AddByNameModal"
import { Box } from "components/common/Box"
import { useDialog } from "@leancodepl/utils"
import { api, apiComponents } from "../../api"
import { Page } from "../../components/common/Page"

export function EmployeesPage() {
    const { isDialogOpen, openDialog, closeDialog } = useDialog()

    const { mutate, isPending } = api.useCreateEmployee({
        invalidateQueries: [api.useAllEmployeesAdmin.key({})],
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
                        <FormattedMessage defaultMessage="Add employee" />
                    </Button>
                </Box>
                <apiComponents.AllEmployeesAdminApiTable />
            </Card>
            <AddByNameModal
                isAdding={isPending}
                isOpen={isDialogOpen}
                title={<FormattedMessage defaultMessage="Add employee" />}
                onAdd={name => {
                    mutate({
                        Email: name + "@leancode.pl",
                        Name: name,
                    })
                }}
                onClose={closeDialog}
            />
        </Page>
    )
}
