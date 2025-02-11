import { styled } from "@pigment-css/react"
import { colors } from "../../.."
import { Text } from "../../Text"

export const FormControlError = styled(Text, { name: "FormControl", slot: "error" })`
  color: ${colors.foreground.danger.primary};

  &:empty {
    display: none;
  }
`
