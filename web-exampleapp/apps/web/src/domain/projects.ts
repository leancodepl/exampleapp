import { UncapitalizeDeep } from "@leancodepl/utils"
import { ProjectDTO } from "../api/cqrs"

export type Project = UncapitalizeDeep<ProjectDTO>
