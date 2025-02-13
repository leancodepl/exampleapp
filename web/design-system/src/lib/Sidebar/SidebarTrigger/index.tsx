import { ComponentPropsWithoutRef, ElementRef, forwardRef } from "react"
import { Button } from "../../Button"
import { IconMenu01 } from "../../icons"
import { useSidebar } from "../SidebarProvider"
import { triggerIconCss } from "./styles"

export const SidebarTrigger = forwardRef<ElementRef<typeof Button>, ComponentPropsWithoutRef<typeof Button>>(
  ({ onClick, ...props }, ref) => {
    const { toggleSidebar } = useSidebar()

    return (
      <Button
        ref={ref}
        data-sidebar="trigger"
        leading={<IconMenu01 className={triggerIconCss} />}
        type="text"
        onClick={event => {
          onClick?.(event)
          toggleSidebar()
        }}
        {...props}
      />
    )
  },
)
