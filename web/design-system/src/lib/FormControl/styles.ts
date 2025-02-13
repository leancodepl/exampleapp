import { styled } from "@pigment-css/react"
import { motion } from "motion/react"
import { Stack } from "../Stack"

export const FormControlBaseRoot = styled("div", { name: "FormControlBase", slot: "root" })({
  display: "flex",
})

export const InputContainer = styled("div", { name: "FormControlBase", slot: "inputContainer" })<{
  error?: boolean
  disabled?: boolean
  size?: "large" | "medium"
}>(({ theme }) => ({
  display: "flex",
  backgroundColor: "inherit",
  flex: "1",
  height: "2.5rem",
  borderRadius: theme.radii.md,
  border: "1px solid",
  borderColor: theme.vars.colors.foreground.default.tertiary,
  paddingInline: theme.spacing["_3"],
  gap: theme.spacing["_2"],
  alignItems: "center",

  "&:focus-within": {
    borderColor: theme.vars.colors.foreground.accent.primary,
    boxShadow: theme.vars.shadows.md,
  },

  variants: [
    {
      props: { error: true },
      style: {
        borderColor: theme.vars.colors.foreground.danger.primary,
        "&:focus-within": {
          boxShadow: theme.vars.shadows.error,
        },
      },
    },
    {
      props: { size: "large" },
      style: {
        height: "3rem",
      },
    },
  ],
}))

export const LabelHeaderContainer = styled(motion.label, { name: "FormControlBase", slot: "labelHeaderContainer" })({
  display: "grid",
})

export const LabelInContent = styled(motion.div, { name: "FormControlBase", slot: "labelInContent" })({
  flex: 0,
  pointerEvents: "none",
})

export const ValueContainer = styled("div", { name: "FormControlBase", slot: "valueContainer" })({
  height: "100%",
  display: "flex",
  flex: "1",
})

export const ValuePosition = styled(Stack, { name: "FormControlBase", slot: "valuePosition" })({
  display: "grid",
  height: "100%",
  flex: "1",
  alignItems: "center",
  "> *": {
    gridColumn: "1 / span 1",
    gridRow: "1 / span 1",
  },
})

export const IconContainer = styled("div", { name: "FormControlBase", slot: "iconContainer" })(({ theme }) => ({
  color: theme.vars.colors.foreground.default.secondary,
  display: "contents",
}))
