import { css, styled } from "@pigment-css/react"
import { TextSize } from "."
import { colors } from "../.."

export const TextRoot = styled("span", { name: "Text", slot: "root" })<{
  size?: TextSize
  bold?: boolean
  italic?: boolean
  ellipsis?: boolean
  capitalize?: boolean
  contents?: boolean
  overflowWrap?: boolean
}>({
  fontFamily: `"Inter", sans-serif`,
  variants: [
    {
      props: { italic: true },
      style: { fontStyle: "italic" },
    },
    {
      props: { contents: true },
      style: { display: "contents" },
    },
    {
      props: { ellipsis: true },
      style: { textOverflow: "ellipsis", overflow: "hidden", whiteSpace: "nowrap" },
    },
    {
      props: { overflowWrap: true },
      style: { overflowWrap: "anywhere" },
    },
    {
      props: { capitalize: true },
      style: { textTransform: "capitalize" },
    },
    {
      props: { size: "display" },
      style: {
        fontSize: "40px",
        fontWeight: "700",
        lineHeight: "56px",
        letterSpacing: "-0.022px",
      },
    },
    {
      props: { size: "headline-large" },
      style: {
        fontSize: "32px",
        fontWeight: "700",
        lineHeight: "40px",
        letterSpacing: "-0.021px",
      },
    },
    {
      props: { size: "headline-medium" },
      style: {
        fontSize: "24px",
        fontWeight: "700",
        lineHeight: "32px",
        letterSpacing: "-0.02px",
      },
    },
    {
      props: { size: "headline-small" },
      style: {
        fontSize: "20px",
        fontWeight: "600",
        lineHeight: "24px",
        letterSpacing: "-0.02px",
      },
    },
    {
      props: { size: "subtitle" },
      style: {
        fontSize: "16px",
        fontWeight: "700",
        lineHeight: "24px",
        letterSpacing: "-0.01px",
      },
    },
    {
      props: { size: "body" },
      style: {
        fontSize: "16px",
        fontWeight: "500",
        lineHeight: "24px",
        letterSpacing: "-0.01px",
      },
    },
    {
      props: { size: "body", bold: true },
      style: {
        fontWeight: "700",
      },
    },
    {
      props: { size: "caption" },
      style: {
        fontSize: "12px",
        fontWeight: "500",
        lineHeight: "16px",
      },
    },
    {
      props: { size: "caption", bold: true },
      style: {
        fontWeight: "700",
      },
    },
    {
      props: { size: "overline" },
      style: {
        fontSize: "12px",
        fontWeight: "500",
        lineHeight: "16px",
        textTransform: "uppercase",
      },
    },
  ],
})

export const colorMap = {
  "default.primary": css`
    color: ${colors.foreground.default.primary};
  `,
  "default.secondary": css`
    color: ${colors.foreground.default.secondary};
  `,
  "danger.primary": css`
    color: ${colors.foreground.danger.primary};
  `,
}
