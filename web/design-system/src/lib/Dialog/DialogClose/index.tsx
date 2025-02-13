import * as DialogPrimitive from "@radix-ui/react-dialog"
import { IconXClose } from "../../icons"
import { DialogCloseButton } from "./styles"

export function DialogClose() {
  return (
    <DialogPrimitive.Close asChild>
      <DialogCloseButton leading={<IconXClose />} type="text" />
    </DialogPrimitive.Close>
  )
}
