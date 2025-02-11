import { ButtonType } from "."
import { attribute } from "../../utils/functions/attribute"

export const dataType = attribute<ButtonType>("type")
export const dataSize = attribute<"large" | "medium">("size")
export const dataIconOnly = attribute<"">("icon-only")
export const dataBlock = attribute<"">("block")
export const dataDisabled = attribute<"">("disabled")
