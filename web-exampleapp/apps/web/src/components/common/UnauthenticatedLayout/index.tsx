import { Outlet } from "react-router-dom"
import { Layout } from "antd"
import { CardWrapper, CenteredContent } from "./styles"

export function UnauthenticatedLayout() {
    return (
        <Layout>
            <CenteredContent>
                <CardWrapper>
                    <Outlet />
                </CardWrapper>
            </CenteredContent>
        </Layout>
    )
}
