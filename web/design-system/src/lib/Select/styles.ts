import { styled } from "@pigment-css/react"
import * as SelectPrimitive from "@radix-ui/react-select"
import { breakpoints, colors, spacing } from "../../utils"
import { FormControlContainer } from "../FormControl"

export const SelectFormControlContainer = styled(FormControlContainer, { name: "Select", slot: "container" })`
  display: flex;
  justify-content: space-between;
  min-width: 100%;

  cursor: pointer;

  [data-state="open"]& {
    border-color: ${colors.foreground.accent.primary};
    border-bottom-right-radius: 0;
    border-bottom-left-radius: 0;
  }

  ${breakpoints.up.xs} {
    width: 400px;
    min-width: unset;
  }

  & > span > span:first-child {
    display: flex;
    gap: ${spacing._2};
    align-items: center;
    overflow: hidden;
  }
`

export const SelectPrimitiveIcon = styled(SelectPrimitive.Icon, { name: "Select", slot: "icon" })`
  color: ${colors.foreground.default.secondary};
`
