import { ReactNode } from "react";
import { Layout } from "antd";
import { NavMenu } from "./NavMenu";
import { ContentWrapper, LayoutWrapper } from "./styles";

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
