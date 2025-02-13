import { styled } from "@pigment-css/react"
import * as SeparatorPrimitive from "@radix-ui/react-separator"

export const DividerRoot = styled(SeparatorPrimitive.Root, { name: "Separator", slot: "root" })(({ theme }) => ({
  backgroundColor: theme.vars.colors.foreground.default.tertiary,
  flexShrink: 0,

  variants: [
    {
      props: { orientation: "horizontal" },
      style: {
        height: "1px",
        width: "100%",
      },
    },
    {
      props: { orientation: "vertical" },
      style: {
        height: "100%",
        width: "1px",
      },
    },
  ],
}))
