import { styled } from "@pigment-css/react"
import { colors, radii } from "../.."

export const Drawer = styled.div`
  display: flex;
  flex-direction: column;
  overflow: hidden;

  border: 1px solid ${colors.foreground.default.tertiary};
  border-radius: ${radii.md};
`
