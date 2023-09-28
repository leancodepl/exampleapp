import { Layout } from "antd";
import { Outlet } from "react-router-dom";
import { CenteredContent } from "./styles";

export function UnauthenticatedLayout() {
    return (
        <Layout>
            <CenteredContent>
                <Outlet />
            </CenteredContent>
        </Layout>
    );
}
