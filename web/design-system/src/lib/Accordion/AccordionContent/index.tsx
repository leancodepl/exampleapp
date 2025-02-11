import { ComponentPropsWithoutRef } from "react"
import * as AccordionPrimitive from "@radix-ui/react-accordion"
import { AccordionPrimitiveContent } from "./styles"

type AccordionContentProps = ComponentPropsWithoutRef<typeof AccordionPrimitive.Content>

export function AccordionContent({ children, ...props }: AccordionContentProps) {
  return <AccordionPrimitiveContent {...props}>{children}</AccordionPrimitiveContent>
}
