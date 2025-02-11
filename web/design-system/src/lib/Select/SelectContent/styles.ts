import { styled } from "@pigment-css/react"
import * as SelectPrimitive from "@radix-ui/react-select"
import { colors, radii } from "../../../utils"

export const SelectPrimitiveContent = styled(SelectPrimitive.Content, { name: "Select", slot: "content" })`
  z-index: 3;

  width: var(--radix-select-trigger-width);
  max-height: var(--radix-select-content-available-height);

  border: 1px solid ${colors.foreground.accent.primary};
  border-color: inherit;
  border-top: none;
  border-bottom-right-radius: ${radii.md};
  border-bottom-left-radius: ${radii.md};
`
