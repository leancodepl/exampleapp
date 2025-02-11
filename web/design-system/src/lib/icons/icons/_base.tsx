/// <reference types="./_base.d.ts" />

import { styled } from "@pigment-css/react"

type IconProps = {
  size?: "illustration" | "small" | "standard"
}

export const IconBase = styled("svg")<IconProps>({
  width: 24,
  height: 24,
  strokeWidth: 1.5,
  flex: "0 0 auto",
  variants: [
    {
      props: { size: "small" },
      style: {
        width: 16,
        height: 16,
      },
    },
    {
      props: { size: "illustration" },
      style: {
        width: 32,
        height: 32,
        strokeWidth: 1.5,
      },
    },
  ],
})
