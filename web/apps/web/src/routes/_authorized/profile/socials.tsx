import { createFileRoute } from "@tanstack/react-router"
import { ProfileOidcFlow } from "../../../auth/flows/ProfileOidc"

export const Route = createFileRoute("/_authorized/profile/socials")({
  component: ProfileOidcFlow,
})
