import { MutableDeep } from "@leancodepl/utils";
import { FormattedMessage } from "react-intl";
import { mkPath } from "./_utils/mkPath";
import { settingsRoute } from "./authRoutes";
import { Redirect } from "./components/common/Redirect";
import { ProjectsPage } from "./pages/projects";
import { SettingsPage } from "./pages/settings";

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
                element: <>Project</>,
            },
            {
                name: "create",
                path: "create/",
                element: <>Create</>,
            },
        ],
    },
    {
        name: "not-found",
        path: "*",
        element: <FormattedMessage defaultMessage="Page not found" />,
    },
] as const;

export const routes = internalRoutes as unknown as MutableDeep<typeof internalRoutes>;

export const path = mkPath(internalRoutes);
