import { styled } from "@pigment-css/react"
import { colors } from "../../.."
import { dataError, dataLabelInHeader } from "../attributes"

export const FormControlLabelWrapper = styled("div", {
  name: "FormControl",
  slot: "label",
})`
  color: ${colors.foreground.default.secondary};

  opacity: 0;

  user-select: none;
  pointer-events: none;

  ${dataLabelInHeader.variant("")} & {
    opacity: 1;

    user-select: text;
    pointer-events: auto;
  }

  ${dataError.variant("")} & {
    color: ${colors.foreground.danger.primary};
  }
`
