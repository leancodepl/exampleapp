import { styled } from "@pigment-css/react"

export const Stack = styled("div", { name: "Stack" })({
  display: "grid",
  grid: `"item" auto / auto`,

  "> *": {
    gridArea: "item",
  },
})
