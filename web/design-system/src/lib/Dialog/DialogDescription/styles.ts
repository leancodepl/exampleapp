import { styled } from "@pigment-css/react"
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { spacing } from "../../.."

export const StyledDialogDescription = styled(DialogPrimitive.DialogDescription, {
  name: "Dialog",
  slot: "description",
})`
  flex: 1 1 auto;
  min-height: 0;
  margin: 0;
  padding: ${spacing._6};
  overflow: auto;
`
