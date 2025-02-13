import { styled } from "@pigment-css/react"
import { colors } from "@web/design-system"

export const Avatar = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  width: 56px;
  height: 56px;

  color: ${colors.foreground.accent.secondary};

  background: ${colors.background.default.secondary};
  border-radius: 100%;
`
