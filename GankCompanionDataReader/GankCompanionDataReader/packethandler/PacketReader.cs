using GankCompanionDataReader.eventHandler;
using GankCompanionDataReader.exception;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GankCompanionDataReader.packethandler
{
    public class PacketReader
    {
        private static AlbionPacketHandler albionPacketHandler;
        private IPartyRepository partyRepository;
        public PacketReader(IPartyRepository partyRepository)
        {
            this.partyRepository = partyRepository;
        }

        public void Start(string url)
        {
            // Retrieve the interfaces list
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            AlbionEventHandler albionEventHandler = new AlbionEventHandler(url, partyRepository);
            albionPacketHandler = new AlbionPacketHandler(albionEventHandler);
            // Scan the list printing every entry
            for (int i = 0; i != allDevices.Count(); ++i)
                DevicePrint(allDevices[i]);
            if (allDevices.Count() < 1)
            {
                throw new NoDeviceFoundException();
            }
            CapturingPackets(allDevices);

        }

        public static void CapturePacket(IList<LivePacketDevice> allDevices)
        {
            PacketDevice selectedDevice = SelectDevice(allDevices);
            // Open the device
            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                Console.WriteLine("Listening on " + selectedDevice.Description + "...");

                // Retrieve the packets
                Packet packet;
                do
                {
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:
                            // Timeout elapsed
                            continue;
                        case PacketCommunicatorReceiveResult.Ok:
                            Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" +
                                              packet.Length);
                            break;
                        default:
                            throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                    }
                } while (true);
            }
        }
        public static PacketDevice SelectDevice(IList<LivePacketDevice> allDevices)
        {
            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (No description available)");
            }

            int deviceIndex = 0;
            do
            {
                Console.WriteLine("Enter the interface number (1-" + allDevices.Count + "):");
                string deviceIndexString = Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out deviceIndex) ||
                    deviceIndex < 1 || deviceIndex > allDevices.Count)
                {
                    deviceIndex = 0;
                }
            } while (deviceIndex == 0);

            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[deviceIndex - 1];
            return selectedDevice;
        }
        public static void CapturingPackets(IList<LivePacketDevice> allDevices)
        {
            PacketDevice selectedDevice = SelectDevice(allDevices);
            // Open the device
            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                Console.WriteLine("Listening on " + selectedDevice.Description + "...");
                communicator.ReceivePackets(0, new HandlePacket(albionPacketHandler.Packethandler));
            }
        }

        // Print all the available information on the given interface
        private static void DevicePrint(IPacketDevice device)
        {
            // Name
            Console.WriteLine(device.Name);

            // Description
            if (device.Description != null)
                Console.WriteLine("\tDescription: " + device.Description);

            // Loopback Address
            Console.WriteLine("\tLoopback: " +
                              (((device.Attributes & DeviceAttributes.Loopback) == DeviceAttributes.Loopback)
                                   ? "yes"
                                   : "no"));

            // IP addresses
            foreach (DeviceAddress address in device.Addresses)
            {
                Console.WriteLine("\tAddress Family: " + address.Address.Family);

                if (address.Address != null)
                    Console.WriteLine(("\tAddress: " + address.Address));
                if (address.Netmask != null)
                    Console.WriteLine(("\tNetmask: " + address.Netmask));
                if (address.Broadcast != null)
                    Console.WriteLine(("\tBroadcast Address: " + address.Broadcast));
                if (address.Destination != null)
                    Console.WriteLine(("\tDestination Address: " + address.Destination));
            }
            Console.WriteLine();
        }

    }
}
