import { attribute } from "../../utils/functions/attribute"

export const dataState = attribute<"collapsed" | "expanded">("state")
export const dataSide = attribute<"left" | "right">("side")
export const dataActive = attribute<"">("active")
