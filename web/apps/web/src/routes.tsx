import { mkPath } from "./_utils/mkPath";
import { MutableDeep } from "./_utils/mutableDeep";

export const signInRoute = "/";
export const signUpRoute = "/signup";

const internalRoutes = [
    {
        name: "projects",
        path: "projects/",
        element: <>Projects</>,
        children: [
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
] as const;

export const routes = internalRoutes as unknown as MutableDeep<typeof internalRoutes>;

export const path = mkPath(internalRoutes);
