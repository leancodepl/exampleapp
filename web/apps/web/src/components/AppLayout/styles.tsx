import { Layout } from "antd"
import styled from "styled-components"

const { Content } = Layout

export const LayoutWrapper = styled(Layout)`
    height: 100%;

    .ant-layout-sider-children {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        height: 100%;
    }
`

export const LogoContainer = styled.div`
    display: flex;
    justify-content: center;
    height: 80px;

    padding: ${({ theme }) => theme.spacing.small};
`

export const ContentWrapper = styled(Content)`
    height: 100%;
    overflow: auto;
`
