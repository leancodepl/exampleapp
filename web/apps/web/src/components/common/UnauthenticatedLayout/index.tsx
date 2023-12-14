import { Layout } from "antd";
import { Outlet } from "react-router-dom";
import { CardWrapper, CenteredContent } from "./styles";

export function UnauthenticatedLayout() {
    return (
        <Layout>
            <CenteredContent>
                <CardWrapper>
                    <Outlet />
                </CardWrapper>
            </CenteredContent>
        </Layout>
    );
}
