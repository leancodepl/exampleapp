import { FormattedMessage } from "react-intl"
import { Button, Card, Flex, Space } from "antd"
import { AddByNameModal } from "components/common/AddByNameModal"
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
                <Space
                    direction="vertical"
                    size="small"
                    style={{
                        width: "100%",
                    }}>
                    <Flex justify="flex-end">
                        <Button onClick={openDialog}>
                            <FormattedMessage defaultMessage="Add employee" />
                        </Button>
                    </Flex>
                    <apiComponents.AllEmployeesAdminApiTable />
                </Space>
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
