import { Card, Layout } from "antd"
import styled from "styled-components"

const { Content } = Layout

export const CenteredContent = styled(Content)`
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
`

export const CardWrapper = styled(Card)`
    width: 100%;
    max-width: 420px;
`
