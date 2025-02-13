import { styled } from "@pigment-css/react"
import { colors, spacing, Text } from "@web/design-system"

export const Error404 = styled(Text)`
  color: ${colors.background.accent.primary};
  font-weight: 300;
  font-size: 104px;
`

export const ErrorContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${spacing._6};
  align-items: center;
  justify-content: center;
  height: 100%;
  min-height: 80svh;
`
