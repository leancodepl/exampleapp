import { createFileRoute } from "@tanstack/react-router"
import { Profile } from "../../components/Profile"

export const Route = createFileRoute("/_authorized/profile")({
  component: Profile,
})
