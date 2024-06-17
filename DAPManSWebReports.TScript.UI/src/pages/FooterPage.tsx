import React from 'react';
import { Icon } from '@rsuite/icons';
import { FaHeart } from 'react-icons/fa';

const FooterPage = () => {
    return (
        <footer className="custom-header">
            <div style={{ textAlign: 'center', color: '#fff', padding:'10px', margin:'10px'}} >
                <p>Developed by [Shipelov]
                    <Icon as={FaHeart} style={{ color: 'red', marginLeft: '5px' }} />
                </p>
            </div>
        </footer>
    );
};
export default FooterPage;