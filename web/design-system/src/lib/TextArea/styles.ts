import TextareaAutosize from "react-textarea-autosize"
import { styled } from "@pigment-css/react"
import { colors, spacing } from "../.."
import { FormControlContainer, FormControlValue } from "../FormControl"

export const TextAreaRoot = styled(TextareaAutosize, { name: "TextArea", slot: "root" })`
  flex: 1;
  height: 100%;
  padding: 0;

  color: ${colors.foreground.default.primary};
  font-weight: 400;
  font-size: 14px;
  line-height: 24px;
  letter-spacing: -0.01px;

  background: none;
  border: none;
  outline: none;

  resize: none;

  &::placeholder {
    color: ${colors.foreground.default.secondary};
  }
`

export const TextAreaFormControlContainer = styled(FormControlContainer)`
  height: auto;
`

export const TextAreaFormControlValue = styled(FormControlValue)`
  padding-block: ${spacing._2};

  align-items: start;
`
