import { Menu } from "antd";
import styled from "styled-components";

export const MenuWithoutBorder = styled(Menu)`
    border-inline-end: unset !important;
`;

export const MenuContainer = styled.div`
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    flex: 1 0 auto;
`;
