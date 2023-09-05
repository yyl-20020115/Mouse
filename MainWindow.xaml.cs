using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using System.Windows.Media.Media3D;

namespace Mouse;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // 相机
    private PerspectiveCamera TheCamera = null;

    // 相机遥控器
    private CameraController cc = null;

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ModelVisual3D visual3d = new ModelVisual3D();
        Model3DGroup group3d = new Model3DGroup();
        visual3d.Content = group3d;
        mainViewport.Children.Add(visual3d);

        DefineCamera(mainViewport);
        DefineLights(group3d);
        DefineModel(group3d);
    }

    // 定义相机
    private void DefineCamera(Viewport3D viewport)
    {
        TheCamera = new PerspectiveCamera();
        TheCamera.FieldOfView = 60;
        cc = new CameraController(TheCamera, viewport, this);
    }

    // 定义光线
    private void DefineLights(Model3DGroup group)
    {
        group.Children.Add(new AmbientLight(Colors.Gray));

        Vector3D direction = new Vector3D(1, -2, -3);
        group.Children.Add(new DirectionalLight(Colors.Gray, direction));

        Vector3D direction2 = new Vector3D(0, -1, 0);
        group.Children.Add(new DirectionalLight(Colors.Gray, direction2));
    }

    // 定义模型
    private void DefineModel(Model3DGroup group)
    {
        // Make the ground.
        MeshGeometry3D groundMesh = new MeshGeometry3D();
        const double wid = 10;
        groundMesh.Positions.Add(new Point3D(-wid, 0, -wid));
        groundMesh.Positions.Add(new Point3D(-wid, 0, +wid));
        groundMesh.Positions.Add(new Point3D(+wid, 0, +wid));
        groundMesh.Positions.Add(new Point3D(+wid, 0, -wid));
        groundMesh.TriangleIndices.Add(0);
        groundMesh.TriangleIndices.Add(1);
        groundMesh.TriangleIndices.Add(2);
        groundMesh.TriangleIndices.Add(0);
        groundMesh.TriangleIndices.Add(2);
        groundMesh.TriangleIndices.Add(3);
        DiffuseMaterial groundMaterial = new DiffuseMaterial(Brushes.DarkGray);
        GeometryModel3D groundModel = new GeometryModel3D(groundMesh, groundMaterial);
        group.Children.Add(groundModel);

        // Make some cubes.
        for (int x = -2; x <= 2; x += 2)
        {
            for (int z = -2; z <= 2; z += 2)
            {
                MeshGeometry3D mesh = MakeCubeMesh(x, 0.5, z, 1);

                byte r = (byte)(128 + x * 50);
                byte g = (byte)(128 + z * 50);
                byte b = (byte)(128 + x * 50);
                Color color = Color.FromArgb(255, r, g, b);
                DiffuseMaterial material = new DiffuseMaterial(
                    new SolidColorBrush(color));

                GeometryModel3D model = new GeometryModel3D(mesh, material);
                group.Children.Add(model);
            }
        }
    }

    // 根据中心位置生成立方体
    private MeshGeometry3D MakeCubeMesh(double x, double y, double z, double w)
    {
        MeshGeometry3D mesh = new MeshGeometry3D();

        // Define the positions.
        w /= 2;
        Point3D[] points =
        {
            new Point3D(x - w, y - w, z - w),
            new Point3D(x + w, y - w, z - w),
            new Point3D(x + w, y - w, z + w),
            new Point3D(x - w, y - w, z + w),
            new Point3D(x - w, y - w, z + w),
            new Point3D(x + w, y - w, z + w),
            new Point3D(x + w, y + w, z + w),
            new Point3D(x - w, y + w, z + w),
            new Point3D(x + w, y - w, z + w),
            new Point3D(x + w, y - w, z - w),
            new Point3D(x + w, y + w, z - w),
            new Point3D(x + w, y + w, z + w),
            new Point3D(x + w, y + w, z + w),
            new Point3D(x + w, y + w, z - w),
            new Point3D(x - w, y + w, z - w),
            new Point3D(x - w, y + w, z + w),
            new Point3D(x - w, y - w, z + w),
            new Point3D(x - w, y + w, z + w),
            new Point3D(x - w, y + w, z - w),
            new Point3D(x - w, y - w, z - w),
            new Point3D(x - w, y - w, z - w),
            new Point3D(x - w, y + w, z - w),
            new Point3D(x + w, y + w, z - w),
            new Point3D(x + w, y - w, z - w),
        };
        foreach (Point3D point in points) mesh.Positions.Add(point);

        // Define the triangles.
        Tuple<int, int, int>[] triangles = new Tuple<int, int, int>[12];
        for (int i = 0; i < triangles.Length; i++)
        {
            int tmp = i % 2 == 0 ? 2 : -2;
            triangles[i] = new Tuple<int, int, int>(
                2 * i, 2 * i + 1, 2 * i + tmp);
        }
        foreach (Tuple<int, int, int> tuple in triangles)
        {
            mesh.TriangleIndices.Add(tuple.Item1);
            mesh.TriangleIndices.Add(tuple.Item2);
            mesh.TriangleIndices.Add(tuple.Item3);
        }

        return mesh;
    }

}
