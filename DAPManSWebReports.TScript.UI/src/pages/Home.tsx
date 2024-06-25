import React from 'react';
import { Container, Content } from 'rsuite';
import Breadcrumbs from '../Models/Breadcrumbs';
import '../App.css';
import 'rsuite/dist/rsuite.min.css';
interface Props {
    path: string;
    updateBreadcrumbs: (breadcrumbs: string[]) => void;
    breadcrumbs: string[];
}

const Home: React.FC<Props> = ({ path, updateBreadcrumbs, breadcrumbs }) =>
{
    let pathArray: string[] = [];
    let newBreadcrumbs: any[] = [];

    const handleHomeClick = async () => {       
        try
        {            
            const newPath = path ? `${path}/Home` : 'ErrorPage';
            newBreadcrumbs = [...breadcrumbs, 'Home'];
            pathArray.push(path, newPath);
            updateBreadcrumbs(newBreadcrumbs);
        }
        catch (error)
        {
            console.error("breadcrumbs", error);
        }
    };

    return (            
        <Container className="container">
            <div onClick ={handleHomeClick}>
                  <Content className="content">
                    <p>Home Page Content!</p>
                </Content>
            </div>
            </Container>
    );
};
export default Home;