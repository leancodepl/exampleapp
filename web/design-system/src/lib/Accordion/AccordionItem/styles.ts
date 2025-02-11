import { styled } from "@pigment-css/react"
import * as AccordionPrimitive from "@radix-ui/react-accordion"
import { colors, radii } from "../../../utils"

export const AccordionPrimitiveItem = styled(AccordionPrimitive.Item)`
  border: 1px solid ${colors.foreground.default.tertiary};

  &:first-of-type {
    border-top-left-radius: ${radii.md};
    border-top-right-radius: ${radii.md};
  }

  &:not(:first-child) {
    margin-top: -1px;
  }

  &:last-of-type {
    border-bottom-right-radius: ${radii.md};
    border-bottom-left-radius: ${radii.md};
  }
`
