import { ReactNode } from "react";
import { NavMenu } from "./NavMenu";
import { ContentWrapper, LayoutWrapper, SiderWrapper } from "./styles";

type AppLayoutProps = {
    children?: ReactNode;
};

export function AppLayout({ children }: AppLayoutProps) {
    return (
        <LayoutWrapper hasSider>
            <SiderWrapper theme="light">
                <NavMenu />
            </SiderWrapper>
            <ContentWrapper>{children}</ContentWrapper>
        </LayoutWrapper>
    );
}
