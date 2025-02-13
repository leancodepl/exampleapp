import { ComponentPropsWithoutRef, forwardRef, ReactNode } from "react"
import { Slot, Slottable } from "@radix-ui/react-slot"
import { dataBlock, dataDisabled, dataIconOnly, dataSize, dataType } from "./attributes"
import { ButtonRoot, Loader } from "./styles"

export type ButtonType = "danger" | "primary" | "secondary" | "tertiary" | "text"
export type ButtonSize = "large" | "medium"

export type ButtonProps = {
  type?: ButtonType
  size?: ButtonSize
  block?: boolean

  asChild?: boolean

  children?: ReactNode
  trailing?: ReactNode
  leading?: ReactNode

  htmlType?: ComponentPropsWithoutRef<"button">["type"]

  isLoading?: boolean
} & Omit<ComponentPropsWithoutRef<"button">, "size" | "type">

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(
  (
    {
      type = "primary",
      size = "medium",
      asChild,
      children,
      trailing,
      leading,
      htmlType,
      isLoading,
      block,
      disabled,
      ...props
    },
    ref,
  ) => {
    const iconOnly = !children && ((!!trailing && !leading) || (!trailing && !!leading))
    const Component = asChild ? Slot : "button"

    return (
      <ButtonRoot
        {...props}
        {...dataBlock(block ? "" : undefined)}
        {...dataType(type)}
        {...dataIconOnly(iconOnly ? "" : undefined)}
        {...dataSize(size)}
        {...dataDisabled(disabled ? "" : undefined)}
        ref={ref}
        as={Component}
        disabled={isLoading || disabled}
        type={htmlType}>
        {leading}
        {isLoading && <Loader />}
        <Slottable>{children}</Slottable>
        {block && isLoading && <Loader shadow />}
        {trailing}
      </ButtonRoot>
    )
  },
)
