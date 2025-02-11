import { Slot, Slottable } from "@radix-ui/react-slot"
import { ReactNode } from "@tanstack/react-router"
import { DrawerIcon, DrawerItemContainer } from "./styles"

type DrawerItemProps = {
  icon?: ReactNode
  details?: ReactNode
  children?: ReactNode
  asChild?: boolean
}

export function DrawerItem({ icon, children, details, asChild }: DrawerItemProps) {
  const Component = asChild ? Slot : "button"

  return (
    <DrawerItemContainer as={Component}>
      {icon && <DrawerIcon>{icon}</DrawerIcon>}
      <Slottable>{children}</Slottable>
      {details}
    </DrawerItemContainer>
  )
}
