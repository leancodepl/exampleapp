import { Card } from "antd";
import { apiComponents } from "../../api";
import { Page } from "../../components/common/Page";

export function ProjectsPage() {
    return (
        <Page>
            <Card>
                <apiComponents.AllProjectsAdminApiTable />
            </Card>
        </Page>
    );
}
