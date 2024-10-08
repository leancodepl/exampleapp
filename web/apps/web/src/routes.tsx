import { FormattedMessage } from "react-intl"
import { EmployeesPage } from "pages/employees"
import { ProjectPage } from "pages/projects/project"
import { MutableDeep } from "@leancodepl/utils"
import { mkPath } from "./_utils/mkPath"
import { Redirect } from "./components/common/Redirect"
import { settingsRoute } from "./kratosRoutes"
import { ProjectsPage } from "./pages/projects"
import { SettingsPage } from "./pages/settings"

const internalRoutes = [
    {
        name: "index",
        path: "",
        element: <Redirect path="/projects" />,
    },
    { name: settingsRoute, path: `${settingsRoute}/`, element: <SettingsPage /> },
    {
        name: "projects",
        path: "projects/",
        children: [
            {
                name: "index",
                path: "",
                element: <ProjectsPage />,
            },
            {
                name: "project",
                path: ":projectId",
                element: <ProjectPage />,
            },
            {
                name: "create",
                path: "create/",
                element: <>Create</>,
            },
        ],
    },
    {
        name: "employees",
        path: "employees/",
        children: [
            {
                name: "index",
                path: "",
                element: <EmployeesPage />,
            },
        ],
    },
    {
        name: "not-found",
        path: "*",
        element: <FormattedMessage defaultMessage="Page not found" />,
    },
] as const

export const routes = internalRoutes as unknown as MutableDeep<typeof internalRoutes>

export const path = mkPath(internalRoutes)
