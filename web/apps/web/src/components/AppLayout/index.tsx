import { ReactNode } from "react";
import { NavMenu } from "./NavMenu";
import { ContentWrapper, LayoutWrapper, SiderWrapper } from "./styles";
import { Layout } from "antd";

const { Sider } = Layout;

type AppLayoutProps = {
    children?: ReactNode;
};

export function AppLayout({ children }: AppLayoutProps) {
    return (
        <LayoutWrapper hasSider>
            <SiderWrapper>
                <Sider theme="light">
                    <NavMenu />
                </Sider>
            </SiderWrapper>
            <ContentWrapper>{children}</ContentWrapper>
        </LayoutWrapper>
    );
}
