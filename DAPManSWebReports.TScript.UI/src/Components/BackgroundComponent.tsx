const BackgroundComponent = () => {
    const contentStyle = {
        backgroundImage: `url(${backgroundImage})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        height: '100vh', // ���������, ��� ������ ���������� �������, ����� ���������� ��������
    };
  return (
    <p>Hello world!</p>
  );
}

export default BackgroundComponent;