import { styled } from "@pigment-css/react"
import { colors } from "../../.."
import { dataLabelInHeader } from "../attributes"

export const FormControlValueWrapper = styled("div", {
  name: "FormControl",
  slot: "value",
})`
  display: grid;
  flex: 1 1 auto;
  align-items: center;
  height: 100%;

  > * {
    grid-row: 1 / span 1;
    grid-column: 1 / span 1;
  }
`

export const FormControlValueLabel = styled("div", {
  name: "FormControl",
  slot: "value-label",
})`
  color: ${colors.foreground.default.secondary};
  font-weight: 400;
  font-size: 14px;
  line-height: 24px;
  letter-spacing: -0.01px;

  opacity: 1;

  user-select: text;
  pointer-events: auto;

  ${dataLabelInHeader.variant("")} & {
    opacity: 0;

    user-select: none;
    pointer-events: none;
  }
`
