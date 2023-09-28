import { Layout } from "antd";
import styled from "styled-components";

const { Content } = Layout;

export const CenteredContent = styled(Content)`
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
`;
