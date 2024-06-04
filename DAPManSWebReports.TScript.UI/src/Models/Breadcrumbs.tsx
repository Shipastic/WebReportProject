import React from 'react';
import { Breadcrumb } from 'rsuite';
import { useNavigate } from 'react-router-dom';
import '../assets/css/Breadcrumbs.css';

interface BreadcrumbsProps {
    breadcrumbs: string[];
}

const Breadcrumbs: React.FC<BreadcrumbsProps> = ({ breadcrumbs }) => {
    const navigate = useNavigate();

    const handleClick = (index: number) => {
        const path = `/${breadcrumbs.slice(1, index + 1).join('/')}`;
        navigate(path);
    };

    return (
        <Breadcrumb className="custom-breadcrumb" style={{ margin: '0px 10px 0px 75px', padding:'0px 0px 0px 10px' }}>
            {breadcrumbs.map((breadcrumb, index) => (
                <Breadcrumb.Item
                    key={index}
                    className={index === breadcrumbs.length - 1 ? 'breadcrumb-item-active' : 'breadcrumb-item'}
                    active={index === breadcrumbs.length - 1}
                    onClick={() => handleClick(index)}
                >
                    {breadcrumb}
                </Breadcrumb.Item>
            ))}
        </Breadcrumb>
    );
};

export default Breadcrumbs;