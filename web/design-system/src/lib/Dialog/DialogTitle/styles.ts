import { styled } from "@pigment-css/react"
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { colors, spacing } from "../../../utils"

export const StyledDialogTitle = styled(DialogPrimitive.DialogTitle, { name: "Dialog", slot: "title" })`
  display: flex;
  flex: 0;
  align-items: center;
  min-height: 64px;
  margin: 0;
  padding: ${spacing._2} ${spacing._4};

  border-bottom: 1px solid ${colors.foreground.default.tertiary};
`
