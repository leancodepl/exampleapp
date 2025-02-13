import { styled } from "@pigment-css/react"
import { colors } from "../../.."
import { Text } from "../../Text"

export const FormControlHint = styled(Text, { name: "FormControl", slot: "hint" })`
  color: ${colors.foreground.default.secondary};

  &:empty {
    display: none;
  }
`
