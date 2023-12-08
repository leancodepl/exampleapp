import { ReactNode } from "react";
import { NavMenu } from "./NavMenu";
import { ContentWrapper, LayoutWrapper } from "./styles";
import { Layout } from "antd";

const { Sider } = Layout;

type AppLayoutProps = {
    children?: ReactNode;
};

export function AppLayout({ children }: AppLayoutProps) {
    return (
        <LayoutWrapper hasSider>
            <Sider theme="light">
                <NavMenu />
            </Sider>
            <ContentWrapper>{children}</ContentWrapper>
        </LayoutWrapper>
    );
}
